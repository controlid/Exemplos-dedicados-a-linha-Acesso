using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlID.USB
{
    public class Futronic : IDisposable
    {
        public delegate void LogInfo(string info);
        public delegate void LogError(Exception erro);

        public static readonly Futronic Device = new Futronic();

        // Garante Singleton!
        private Futronic()
        {
        }

        const string MSGAPI1 = "Futronic: Inicializando leitor biométrico";
        const string MSGAPI1b = "Futronic: Leitor não conectado";
        const string MSGAPI2 = "Futronic: Finalizando leitor biométrico";
        const string MSGAPI3 = "Futronic: Não é possível obter imagem com o leitor desconectado";
        const string MSGAPI4 = "Futronic: Obtendo imagem";
        const string MSGAPI5 = "Futronic: Aguardando dedo";
        const string MSGAPI6 = "Futronic: Definindo leds para G {0} e R {1}";
        const string MSGAPI7 = "Futronic: Obtendo status dos leds";
        const string MSGAPI8 = "Futronic: Obtendo imagem completa";
        const string MSGAPI9 = "Futronic: Sem dedo";
        const string MSGAPI10 = "Futronic: Cancelado";
        const string MSGAPI11 = "Futronic: Aguardando remover o dedo";

        #region API Futronic

        struct _FTRSCAN_FAKE_REPLICA_PARAMETERS
        {
            public bool bCalculated;
            public int nCalculatedSum1;
            public int nCalculatedSumFuzzy;
            public int nCalculatedSumEmpty;
            public int nCalculatedSum2;
            public double dblCalculatedTremor;
            public double dblCalculatedValue;
        }
        struct _FTRSCAN_FRAME_PARAMETERS
        {
            public int nContrastOnDose2;
            public int nContrastOnDose4;
            public int nDose;
            public int nBrightnessOnDose1;
            public int nBrightnessOnDose2;
            public int nBrightnessOnDose3;
            public int nBrightnessOnDose4;
            public _FTRSCAN_FAKE_REPLICA_PARAMETERS FakeReplicaParams;
            public _FTRSCAN_FAKE_REPLICA_PARAMETERS Reserved;
        }
        struct _FTRSCAN_IMAGE_SIZE
        {
            public int nWidth;
            public int nHeight;
            public int nImageSize;
        }

        public class ImageResult
        {
            public Bitmap Image;
            public byte[] Data;
        }

        [DllImport("ftrScanAPI.dll")]
        static extern bool ftrScanIsFingerPresent(IntPtr ftrHandle, out _FTRSCAN_FRAME_PARAMETERS pFrameParameters);
        [DllImport("ftrScanAPI.dll")]
        static extern IntPtr ftrScanOpenDevice();
        [DllImport("ftrScanAPI.dll")]
        static extern void ftrScanCloseDevice(IntPtr ftrHandle);
        [DllImport("ftrScanAPI.dll")]
        static extern bool ftrScanSetDiodesStatus(IntPtr ftrHandle, byte byGreenDiodeStatus, byte byRedDiodeStatus);
        [DllImport("ftrScanAPI.dll")]
        static extern bool ftrScanGetDiodesStatus(IntPtr ftrHandle, out bool pbIsGreenDiodeOn, out bool pbIsRedDiodeOn);
        [DllImport("ftrScanAPI.dll")]
        static extern bool ftrScanGetImageSize(IntPtr ftrHandle, out _FTRSCAN_IMAGE_SIZE pImageSize);
        [DllImport("ftrScanAPI.dll")]
        static extern bool ftrScanGetImage(IntPtr ftrHandle, int nDose, byte[] pBuffer);

        #endregion

        #region Variáveis, Propriedades, Inicialização e Controle de status

        private IntPtr device;
        private bool waitFinger = false; // somente se estiver no loop do IsFinger
        private int lastContrast = 0; // Valor do ultimo contraste capturado

        public static int dose = 4; // Dose 0 - dark dose (absense of light). Doses from 1 up to 3 - turn on light (1=min, 4=max)
        public static int delay = 50; // tempo usado nos sleeps
        public static int minContrast = 800; // contraste minimo que indica a existencia de um dedo
        public static bool crop = true; // Indica se é para fazer CROP da imagem (compatibilidade com versão antiga)

        public bool WaitingFinger { get { return waitFinger; } }
        public bool Connected { get { return (device != IntPtr.Zero); } }

        public event LogInfo onInfo = null;
        public event LogInfo onInfoAppend = null;
        public event LogError onError = null;

        public bool Init()
        {
            try
            {
                if (Connected)
                    Dispose();

                onInfo?.Invoke(MSGAPI1);
                device = ftrScanOpenDevice();
                if (device == IntPtr.Zero)
                    onInfo?.Invoke(MSGAPI1b);

                return device != IntPtr.Zero;
            }
            catch (Exception ex)
            {
                device = IntPtr.Zero;

                if (ex.Message.Contains("HRESULT:"))
                    ex = new Exception("Provável execução em 64bit chamando DLL 32bits, force o uso em 32bits x86", ex);

                if (onError == null)
                    throw ex;
                else
                    onError(ex);

                return false;
            }
        }

        public void Dispose()
        {
            try
            {
                if (Connected)
                {
                    AbortFingerDetect();
                    onInfo?.Invoke(MSGAPI2);
                    ftrScanCloseDevice(device);
                }
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }
            device = IntPtr.Zero;
        }

        #endregion

        #region "Obter imagem / Detecção de erro"

        public bool IsNotFinger(TimeSpan timeout)
        {
            try
            {
                onInfo?.Invoke(MSGAPI11);
                waitFinger = true;
                var t = new _FTRSCAN_FRAME_PARAMETERS();
                DateTime dt = DateTime.Now;
                while (waitFinger && DateTime.Now.Subtract(dt) <= timeout)
                {
                    onInfoAppend?.Invoke("!");
                    if (ftrScanIsFingerPresent(device, out t))
                        Thread.Sleep(delay);
                    else
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
                return false;
            }
            finally
            {
                waitFinger = false;
            }
        }

        /// <summary>
        /// Detecta a existencia de um dedo no sensor
        /// </summary>
        public bool IsFinger(TimeSpan timeout, Action waiting = null)
        {
            try
            {
                onInfo?.Invoke(MSGAPI5);
                waitFinger = true;
                var t = new _FTRSCAN_FRAME_PARAMETERS();
                int n = 1;
                bool hasTime = true;
                DateTime dt = DateTime.Now;
                while (waitFinger && hasTime)
                {
                    hasTime = DateTime.Now.Subtract(dt) <= timeout;
                    if (ftrScanIsFingerPresent(device, out t))
                    {
                        if (t.nContrastOnDose2 > minContrast || !hasTime) // garante que vai pegar o dedo por pior que seja no tempo maximo
                        {
                            onInfoAppend?.Invoke(" D" + n + ": " + t.nContrastOnDose2);
                            n++;
                            if(!hasTime)
                            {
                                onInfoAppend?.Invoke(" D" + n + ": " + t.nContrastOnDose2 + " Timeout");
                                return true;
                            }
                            else if (n > 4 ) 
                                return true;
                        }
                        else
                        {
                            lastContrast = t.nContrastOnDose2;
                            onInfoAppend?.Invoke("-" + t.nContrastOnDose2);
                        }
                    }
                    else
                        onInfoAppend?.Invoke(".");

                    waiting?.Invoke();

                    Thread.Sleep(delay);
                }
                onInfo?.Invoke(MSGAPI9);
                return false;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
                return false;
            }
            finally
            {
                waitFinger = false;
            }
        }

        public void AbortFingerDetect()
        {
            if (waitFinger)
            {
                waitFinger = false;
                onInfo?.Invoke(MSGAPI10);
                Thread.Sleep(delay); // Garante cancelamento quando chamado em outra thead
            }
        }

        public ImageResult GetFingerprint()
        {
            try
            {
                if (!Connected)
                {
                    onInfo?.Invoke(MSGAPI3);
                    return null;
                }

                onInfo?.Invoke(MSGAPI8);

                var t = new _FTRSCAN_IMAGE_SIZE();
                ftrScanGetImageSize(device, out t);

                byte[] arr = new byte[t.nImageSize];
                ftrScanGetImage(device, dose, arr);

                int cx = crop ? 40 : 0;
                int cy = crop ? 60 : 0;
                int Width = crop ? t.nWidth - 80 : t.nWidth;
                int Height = crop ? t.nHeight - 80 : t.nHeight;
                int i = 0;

                // Em Crop: 320-80 x 480-80 = > 96000 bytes
                var ir = new ImageResult()
                {
                    Data = new byte[Width * Height],
                    Image = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                };

                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        byte a = (byte)(0xFF - arr[((y + cy) * t.nWidth) + (x + cx)]);
                        // a = (byte)x; // teste Gray Scale
                        ir.Data[i] = a;
                        ir.Image.SetPixel(x, y, Color.FromArgb(a, a, a));
                        i++;
                    }
                }
                return ir;
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
                return null;
            }
        }

        #endregion

        #region "Leds"

        public void SetDiodesStatus(bool green, bool red)
        {
            try
            {
                onInfo?.Invoke(string.Format(MSGAPI6, green ? "OK" : "OFF", red ? "OK" : "OFF"));
                ftrScanSetDiodesStatus(device, (byte)(green ? 255 : 0), (byte)(red ? 255 : 0));
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }
        }

        public void GetDiodesStatus(out bool green, out bool red)
        {
            try
            {
                onInfo?.Invoke(MSGAPI7);
                ftrScanGetDiodesStatus(device, out green, out red);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
                green = red = false;
            }
        }

        #endregion
    }
}
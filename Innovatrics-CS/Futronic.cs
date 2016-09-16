using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace ControlID.USB
{
    public class Futronic : IDisposable
    {
        #region "API"

        struct _FTRSCAN_FAKE_REPLICA_PARAMETERS
        {
            bool bCalculated;
            int nCalculatedSum1;
            int nCalculatedSumFuzzy;
            int nCalculatedSumEmpty;
            int nCalculatedSum2;
            double dblCalculatedTremor;
            double dblCalculatedValue;
        }

        struct _FTRSCAN_FRAME_PARAMETERS
        {
            int nContrastOnDose2;
            int nContrastOnDose4;
            int nDose;
            int nBrightnessOnDose1;
            int nBrightnessOnDose2;
            int nBrightnessOnDose3;
            int nBrightnessOnDose4;
            _FTRSCAN_FAKE_REPLICA_PARAMETERS FakeReplicaParams;
            _FTRSCAN_FAKE_REPLICA_PARAMETERS Reserved;
        }

        struct _FTRSCAN_IMAGE_SIZE
        {
            public int nWidth;
            public int nHeight;
            public int nImageSize;
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

        IntPtr device;

        #endregion

        volatile bool working = false;
        public bool Working
        {
            get { return working; }
            set { working = value; }
        }

        public bool Init()
        {
            if (!Connected)
                device = ftrScanOpenDevice();
            return Connected;
        }

        public bool Connected
        {
            get { return (device != IntPtr.Zero); }
        }

        public void Dispose()
        {
            if (Connected)
                ftrScanCloseDevice(device);
                
            device = IntPtr.Zero;
        }

        public bool IsFinger
        {
            get
            {
                var t = new _FTRSCAN_FRAME_PARAMETERS();
                return ftrScanIsFingerPresent(device, out t);
            }
        }

        public void GetDiodesStatus(out bool green, out bool red)
        {
            ftrScanGetDiodesStatus(device, out green, out red);
        }

        public void SetDiodesStatus(bool green, bool red)
        {
            ftrScanSetDiodesStatus(device, (byte)(green ? 255 : 0), (byte)(red ? 255 : 0));
        }

        public byte[] GetFingerprint(out int width, out int height)
        {
            int timeout = 0;
            working = true;
            while (timeout < 100 && working)
            {
                Dispose();
                if (Init())
                {
                    if (IsFinger)
                    {
                        var bt = GetRawImage(out width, out height);
                        if (bt != null)
                            return bt;
                    }
                }
                Thread.Sleep(100);
                timeout++;
            }
            width = 0;
            height = 0;
            working = false;
            return null;
        }

        public byte[] GetRawImage(out int width, out int height)
        {
            if (!Connected)
            {
                width = 0;
                height = 0;
                return null;
            }
            var t = new _FTRSCAN_IMAGE_SIZE();
            ftrScanGetImageSize(device, out t);
            byte[] arr = new byte[t.nImageSize];
            byte[] cropped_arr = new byte[(t.nWidth - 80) * (t.nHeight - 80)];
            ftrScanGetImage(device, 4, arr);

            width = t.nWidth - 80;
            height = t.nHeight - 80;
            // 320-80 x 480-80 = > 96000 bytes
            int k = 0;
            long sum = 0;
            for (int i = 0; i < arr.Length - 30 * t.nWidth; i += t.nWidth)
            {
                if (i > 50 * t.nWidth)
                {
                    for (int j = 40; j < t.nWidth - 40; j++)
                    {
                        cropped_arr[k] = (byte)(255 - (int)arr[j + i]);
                        sum += cropped_arr[k];
                        k++;
                    }
                }
            }
            long media = sum / 96000l;
            if (media > 200) // é muita área branca, quase não leu nada
                return null;

            return cropped_arr;
        }

        public Bitmap ExportBitMap()
        {
            if (!Connected)
                return null;

            var t = new _FTRSCAN_IMAGE_SIZE();
            ftrScanGetImageSize(device, out t);
            byte[] arr = new byte[t.nImageSize];
            ftrScanGetImage(device, 4, arr);

            var b = new Bitmap(t.nWidth, t.nHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            for (int x = 0; x < t.nWidth; x++)
            {
                for (int y = 0; y < t.nHeight; y++)
                {
                    int a = 255 - arr[y * t.nWidth + x];
                    b.SetPixel(x, y, Color.FromArgb(a, a, a));
                }
            }
            return b;
        }
    }
}

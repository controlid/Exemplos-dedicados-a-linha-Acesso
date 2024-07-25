// Parametros opcionais

var time = process.argv[2]
if(!time)
	time = 0;

var ip = process.argv[3]
if (!ip)
	ip = '192.168.0.129'

var user = process.argv[4]
if (!user)
    user = 'admin'

var pass = process.argv[5]
if (!pass)
    pass = 'admin'

var querystring = require('querystring');
var http = require('http');

function request(args, cb) {

	var post_data = JSON.stringify(args.data);

	var qs = ''
	if (args.querystring)
		qs = '?' + querystring.stringify(args.querystring)

	var post_options = {
		host: ip,
		port: '80',
		path: '/' + args.command + '.fcgi' + qs,
		method: 'POST',
		headers: {
			'Content-Type': 'application/json',
			'Content-Length': post_data.length
		}
	};

	var post_req = http.request(post_options, function(res) {
		res.setEncoding('utf8');
		res.on('data', function (chunk) {
			if (cb)
				cb(JSON.parse(chunk))
		});
	});

	post_req.write(post_data);
	post_req.end();
}


var count = time/1000;
var log = setInterval(function(){
	if(count > 1)
		console.log('0' + count--);
	else{
		console.log('GoGoGo');
		clearInterval(log);
	}
}, 900);

setTimeout(function(){
	request({
		command: 'login',
		data: {login: user, password: pass}
	}, function (data) {
		request({
			command: 'buzzer_buzz',
			data: {frequency: 4000, duty_cycle: 50, timeout: 100},
			querystring: {session: data.session}
		});
		request({
			command: 'execute_actions',
			data: {actions: [{action: 'door', parameters: 'door=1'}]}, // iDAccess/iDFit/iDBox/iDUHF
			//data:  {action: "sec_box", parameters: "id=65793, reason=3"}]}, //iDFlex/iDAccess Pro/iDAccess Nano/iDUHF
			querystring: {session: data.session}
		})
	})
}, time);

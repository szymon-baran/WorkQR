// k6 run k6-load-tests.js

import http from 'k6/http';
import { sleep } from 'k6';

export let options = {
    stages: [
        { duration: '30s', target: 20000 },
        { duration: '60s', target: 20000 },
        { duration: '30s', target: 0 }
    ]
};

let strGlobalUrl = 'localhost:5001';
let params = {
    headers: {
        'Content-Type': 'application/json',
        'dataType': 'json'
    }
};

export function setup() {
    let loginUrl = 'https://' + strGlobalUrl + '/Auth/login';
    let data = JSON.stringify({
        username: 'sbaran',
        password: 'Admin1!',
        rememberMe: true
    });

    let res = http.post(loginUrl, data, params);
    let json = JSON.parse(res.body);
    params.headers['authorization'] = 'Bearer ' + json.token;
}

export default function () {
    let userWorktimeEventsToday = 'https://' + strGlobalUrl + '/WorktimeEvent/getUserWorktimeEventsToday';

    http.get(userWorktimeEventsToday, params);
    sleep(1);
}

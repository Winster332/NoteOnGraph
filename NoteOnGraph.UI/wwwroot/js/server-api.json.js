app.server = {
    host: {
        api_url: "https://localhost:5001/api"
    },
    rest: {
        httpDelete: function (url, callback, id) {
            var req = new XMLHttpRequest();
            req.open("DELETE", url + '/' + id, true);
            req.onload = function () {
                callback(req);
            };
            req.send(null);
        },
        httpPost: function(url, callback, data) {
            var json = JSON.stringify(data);

            var req = new XMLHttpRequest();
            req.open("POST", url, true);
            req.setRequestHeader('Content-type','application/json; charset=utf-8');
            req.onload = function () {
                callback(req);
            }
            req.send(json); 
        },
        httpGet: function (url, callback) {
            var req = new XMLHttpRequest();
            req.open("GET", url, true);
            req.onload = function () {
                callback(req);
            };
            req.send(null);
        },
        httpPut: function (url, callback, data) {
            var req = new XMLHttpRequest();
            req.open("PUT", url, true);
            req.setRequestHeader('Content-type','application/json; charset=utf-8');
            req.onload = function () {
                callback(req);
            };
            var json = JSON.stringify(data);
            req.send(json);
        }
    },
    api: {
        projects: {
            createProjectInRoot: function(callback, project) {
                app.server.rest.httpPut(app.server.host.api_url + '/projects/createProjectInRoot', function (req) {
                    callback(req);
                }, project);
            },
            removeProject: function(callback, id) {
                app.server.rest.httpDelete(app.server.host.api_url + '/projects/removeProject', function (req) {
                    callback(req);
                }, id);
            },
            getFiles: function (callback) {
                app.server.rest.httpGet(app.server.host.api_url + '/projects/getProjects', function (req) {
                    callback(JSON.parse(req.response));
                });
            },
            getProjects: function (callback) {
                app.server.rest.httpGet(app.server.host.api_url + '/projects/getProjects', function (req) {
                    callback(JSON.parse(req.response));
                });
            }
        },
        server: {
            getVersion: function (callback) {
                app.server.rest.httpGet(app.server.host.api_url + '/server/getVersion', function (req) {
                    callback(req.responseText);
                });
            }
        }
    }
};
﻿const { createProxyMiddleware } = require('http-proxy-middleware');

const context = [
    "/mobu",
];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: 'https://localhost:7273',
        secure: false
    });

    app.use(appProxy);
};

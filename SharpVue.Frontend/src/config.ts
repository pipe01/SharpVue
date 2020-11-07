import { App } from 'vue';

import config from "./config.json";

require("bootswatch/dist/" + config.theme + "/bootstrap.min.css")

document.title = config.appName + " documentation";

export const plugin = {
    install(app: App) {
        app.provide("appName", config.appName);
    }
}
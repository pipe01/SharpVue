import { App } from 'vue';

import config from "./gen/config";

document.title = config.appName + " documentation";

export const plugin = {
    install(app: App) {
        app.provide("appName", config.appName);
        app.provide("config", config);
    }
}
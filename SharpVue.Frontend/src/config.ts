import { App } from 'vue';

import data from "./data";

document.title = data.config.appName + " documentation";

export const plugin = {
    install(app: App) {
        app.provide("appName", data.config.appName);
        app.provide("config", data.config);
    }
}

export interface Configuration {
    appName: string;
    dark: boolean;
    logoImage: string | null;
    showAppName: boolean;
}

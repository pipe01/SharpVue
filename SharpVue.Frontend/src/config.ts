import { App } from 'vue';

import data from "./data";

document.title = data.value.config.appName + " documentation";

export const plugin = {
    install(app: App) {
        app.provide("appName", data.value.config.appName);
        app.provide("config", data.value.config);
    }
}

export interface Configuration {
    appName: string;
    dark: boolean;
    logoImage: string | null;
    showAppName: boolean;
}

if (process.env.NODE_ENV != "development") {
    module.exports = {
        css: {
            extract: false,
        },
        productionSourceMap: false,
        configureWebpack: {
            optimization: {
                splitChunks: false
            }
        }
    }
}

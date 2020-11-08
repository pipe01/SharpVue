if (process.env.NODE_ENV != "development") {
    module.exports = {
        css: {
            extract: false,
        },
        configureWebpack: {
            optimization: {
                splitChunks: false
            }
        }
    }
}

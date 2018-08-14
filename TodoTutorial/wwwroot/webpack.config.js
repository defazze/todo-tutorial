var path = require("path");
var webpack = require("webpack");

module.exports = {
  devtool: "eval",
  mode: "development",
  entry: ["./src/index"],
  plugins: [new webpack.HotModuleReplacementPlugin()],
  devServer: {
    port: 3000
  },
  module: {
    rules: [
      {
        use: ["babel-loader"],
        include: path.join(__dirname, "src")
      }
    ]
  }
};

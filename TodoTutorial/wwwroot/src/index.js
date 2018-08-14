var React = require("react");
var { render } = require("react-dom");

render(
  React.createElement("p", null, "Hello world!"),
  document.getElementById("root")
);

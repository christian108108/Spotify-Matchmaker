var React = require('react');
var ReactDOM = require('react-dom')

var Landing = require('./components/Landing')
require('./index.css')

ReactDOM.render(
  <Landing/>,
  document.getElementById('app')
)
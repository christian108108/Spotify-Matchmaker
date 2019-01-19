const React = require('react');

require('./Landing.css')

class Landing extends React.Component {
  render() {
    return (
      <header className="main-header">
        <div className="header-content">
          <h1>Aux are us</h1>
          <p className='tagline'>Something catchy goes here</p>
          <button className='btn landing-btn'>Let's get started</button>
        </div>
      </header>
    )
  }
}

module.exports = Landing
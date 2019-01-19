const React = require('react');

require('./Landing.css')

var Modal = require('./Modal')

class Landing extends React.Component {
  constructor(props) {
    super();

    this.state = {show: false};
    this.showModal = this.showModal.bind(this)
    this.hideModal = this.hideModal.bind(this)
  }

  showModal() {
    this.setState({show: true});
  };

  hideModal() {
    this.setState({show: false})
  }

  render() {
    return (
      <header className="main-header">
        <div className="header-content">
          <h1>Aux are us</h1>
          <p className='tagline'>Something catchy goes here</p>
          <Modal show={this.state.show} handleClose={this.hideModal}>
            <button type="button" className='btn'>Yes</button>
            <button type="button" className='btn'>No</button>
          </Modal>
          <button type="button" className='btn landing-btn' onClick={this.showModal}>Let's get started</button>
        </div>
      </header>
    )
  }
}

module.exports = Landing
var React = require('react');

class ShowParty extends React.Component {

  render() {
    return (
      <footer className="modal-card-foot">
        <p>Your Party code is {this.props.partycode}</p>
      </footer>
    )
  }
}

module.exports = ShowParty
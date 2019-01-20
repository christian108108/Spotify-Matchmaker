var React = require('react');

class PartyForm extends React.Component {
  constructor(props){
    super()
    this.submitPartyCode = this.submitPartyCode.bind(this)
  }
  submitPartyCode(e) {
    e.preventDefault();
    console.log(e)
  }

  render() {
    return (
      <footer className="modal-card-foot">
        <form onSubmit={(e) => this.submitPartyCode(e)}>
          <div className="field">
            <label className="label">Party Code</label>
            <div className="control">
              <input className="input" type="text" placeholder="Enter a Party Code" />
            </div>
          </div>
          <div className="field is-grouped">
            <div className="control">
              <button className="button is-link">Submit</button>
            </div>
            <div className="control">
              <button className="button is-text">Cancel</button>
            </div>
          </div>
        </form>
      </footer>
    )
  }
}

module.exports = PartyForm
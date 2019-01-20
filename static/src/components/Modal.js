var React = require('react')
var PartyForm = require('./PartyForm')
var ShowParty = require('./ShowParty')
require('./Modal.css')

class Modal extends React.Component {
  constructor(props){
    super()
    this.partyCodes = []
    this.state = {noAux: false, generateCode: 0, showCode: false};
    this.showPartyCodeForm = this.showPartyCodeForm.bind(this)
    this.generatePartyCode = this.generatePartyCode.bind(this)
    this.showPartyCode = this.showPartyCode.bind(this)
  }
  
  showPartyCodeForm() {
    this.setState({noAux: !this.state.noAux})
  }

  generatePartyCode() {
    this.setState({generateCode: Math.floor(Math.random() * 900000)+1})
  }

  showPartyCode() {
    this.setState({showCode: !this.state.showCode})
    this.generatePartyCode()
    this.partyCodes.push(this.state.generateCode)
  }

  render() {
    const showHideClassName = this.props.show ? "modal display-block" : "modal display-none"

    return (
      <div className={showHideClassName}>
        <div className="modal-background"></div>
        <div className="modal-card">
          <header className="modal-card-head">
            <p className="modal-card-title title is-2">Got Aux?</p>
          </header>
          <section className="modal-card-body">
            <div className='buttons'>
              <button type="button" className='button is-primary is-large' onClick={this.showPartyCode}>Yes</button>
              <button type="button" className='button is-danger is-large' onClick={this.showPartyCodeForm}>No</button>
            </div>
          </section>
          {this.state.noAux && <PartyForm partycodes = {this.partyCodes}/>}
          {this.state.showCode && <ShowParty partycode= {this.state.generateCode} />}
        </div>
        <button className="modal-close is-large" onClick={this.props.handleClose} aria-label="close"></button>
      </div>
    )
  }

  
}

module.exports = Modal
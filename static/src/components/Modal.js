var React = require('react')
require('./Modal.css')

const Modal = ({handleClose, show, children }) => {
  const showHideClassName = show ? "modal display-block" : "modal display-none"

  return (
    <div className={showHideClassName}>
      <div className="modal-background"></div>
      <div className="modal-card">
        <header className="modal-card-head">
          <p className="modal-card-title title is-2">Got Aux?</p>
        </header>
        <section className="modal-card-body">
          {children}
        </section>
      </div>
      <button className="modal-close is-large" onClick={handleClose} aria-label="close"></button>
    </div>
  )
}

module.exports = Modal
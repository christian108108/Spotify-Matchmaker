var React = require('react')

const Modal = ({handleClose, show, children }) => {
  const showHideClassName = show ? "modal display-block" : "modal display-none"

  return (
    <div className={showHideClassName}>
<<<<<<< HEAD
      <div className="modal-background"></div>
      <div className="modal-card">
        <header className="modal-card-head">
          <p className="modal-card-title">Got Aux?</p>
        </header>
        <section className="modal-card-body">
          {children}
        </section>
      </div>
      <button className="modal-close is-large" onClick={handleClose} aria-label="close"></button>
=======
      <section className="modal-main">
        {children}
        <button onClick={handleClose} className='btn landing-btn'>close</button>
      </section>
>>>>>>> Update static side to include a modal that will appear when a button is clicked
    </div>
  )
}

module.exports = Modal
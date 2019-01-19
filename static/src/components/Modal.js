var React = require('react')

const Modal = ({handleClose, show, children }) => {
  const showHideClassName = show ? "modal display-block" : "modal display-none"

  return (
    <div className={showHideClassName}>
      <section className="modal-main">
        {children}
        <button onClick={handleClose} className='btn landing-btn'>close</button>
      </section>
    </div>
  )
}

module.exports = Modal
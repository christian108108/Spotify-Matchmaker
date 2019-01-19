var React = require('react')

const Modal = ({handleClose, show, children }) => {
  const showHideClassName = show ? "modal display-block" : "modal display-none"

  return (
    <div className={showHideClassName}>
      <div className='modal-dialog'>
        <section className="modal-content">
          <section className="modal-header">
            <h5>Do you have the aux?</h5>
            <button onClick={handleClose} className='close'>x</button>
          </section>
          <section className="modal-body">
            {children}
          </section>
        </section>
      </div>
    </div>
  )
}

module.exports = Modal
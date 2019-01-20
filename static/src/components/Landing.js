const React = require('react');

require('./Landing.css')

var Modal = require('./Modal')


class Landing extends React.Component {
  constructor(props) {
    super();

    this.state = {show: false};
    this.showModal = this.showModal.bind(this)
    this.hideModal = this.hideModal.bind(this)

    const params = this.getHashParams();
    const token = params.access_token;

    if (token) {
      console.log(token)
    }
  }

  getHashParams() {
    var hashParams = {};
    var e, r = /([^&;=]+)=?([^&;]*)/g,
        q = window.location.hash.substring(1);
    e = r.exec(q)
    while (e) {
       hashParams[e[1]] = decodeURIComponent(e[2]);
       e = r.exec(q);
    }
    return hashParams;
  }

  showModal() {
    this.setState({show: true});
  };

  hideModal() {
    this.setState({show: false})
  }

  render() {
    return (
      <section className="hero is-fullheight">
        <div className="hero-body">
          <div className="container">
            <h1 className="title is-1">Spotify Matchmaker</h1>
            <h5 className='subtitle '>Never worry about playlists again</h5>
            <Modal show={this.state.show} handleClose={this.hideModal} />
            <div className='buttons'>
            <button type="button" className='button is-success is-outlined is-medium' onClick={this.showModal}>Let's get started</button>
            <a href='http://localhost:8888/login' className='button is-primary is-outlined is-medium'>Login to Spotify</a>
            </div>
          </div>
        </div>
      </section>
    )
  }
}

module.exports = Landing
package rest

import (
	"net/http"

	"github.com/gorilla/mux"
)

func ServeAPI() {
	mux := mux.NewRouter()
	mux.Methods("GET").Path("/music").HandlerFunc(serveSpotify)
	mux.Methods("POST").Path("/callback").HandlerFunc(callback)
}

func serveSpotify(w http.ResponseWriter, r *http.Request) {
	request, err := http.NewRequest("GET", "https://accounts.spotify.com/authorize", nil)
	if err != nil {
		panic("error while creating get request to the authorize point")
	}
	client := http.Client{}
	res, err := client.Do(request)
	if err != nil {
		panic("error while sending request to server")
	}

	http.Redirect(w, r, res.Request.URL.String(), http.StatusFound)
}

func callback(w http.ResponseWriter, r *http.Request) {

}

package main

import (
	b64 "encoding/base64"
	"errors"
	"fmt"
	"html/template"
	"net/http"

	"github.com/gorilla/mux"
)

const (
	clientID       = "475b4f5384194becb712ad90112be788"
	clientSecretID = "1aae388815834169a26f4d5e04259101"
	redirect_URI   = "http://localhost:8182/callback"
	respType       = "code"
	scopes         = "user-top-read"
	grand_type     = "refresh_token"
)

var (
	key = ""
)

func main() {
	r := mux.NewRouter()
	r.Methods("GET").Path("/music").HandlerFunc(serveSpotify)
	r.Methods("GET").Path("/callback").HandlerFunc(callback)
	// Serve the static files and templates from the static directory
	http.ListenAndServe(":8182", r)

}

func serveSpotify(w http.ResponseWriter, r *http.Request) {
	var res *http.Response
	var err error
	spotifyURI := fmt.Sprintf("https://accounts.spotify.com/authorize?client_id=%s&response_type=%s&redirect_uri=%s&scope=%s", clientID, respType, redirect_URI, scopes)
	if res, err = http.Get(spotifyURI); err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
	}
	http.Redirect(w, r, res.Request.URL.String(), http.StatusFound)
}

func callback(w http.ResponseWriter, r *http.Request) {
	key = r.FormValue("code")
	if rejection := r.FormValue("error"); rejection != "" {
		err := errors.New("No authorization found")
		fmt.Fprintf(w, "Sorry but you did not authorize accses. Try later. Error: %s", err)
	}
	fmt.Println(key)
	templates := template.Must(template.ParseFiles("./static/index.html"))
	templates.Execute(w, nil)

	spotifyServURI := fmt.Sprintf("https://accounts.spotify.com/api/token?grand_type=%s&code=%s&redirect_uri=%s", grand_type, key, redirect_URI)
	request, err := http.NewRequest("POST", spotifyServURI, nil)
	if err != nil {
		panic("Error exchaging tokens. Please try later")
	}
	base64Key := b64.StdEncoding.EncodeToString([]byte(clientSecretID))
	strEnc := fmt.Sprintf("Basic" + clientID + ":" + base64Key)

	request.Header.Set("Authorization", strEnc)
	//Authorization: Basic *<base64 encoded client_id:client_secret>*
	client := http.Client{}
	if res, err := client.Do(request); err != nil {
		panic("Error calling endpoint. Try again")
	} else {
		accseToke := res.Header.Get("StatusCode")
		fmt.Println(accseToke)
	}

}

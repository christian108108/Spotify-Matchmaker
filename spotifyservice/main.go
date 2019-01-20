package main

import (
	"Spotify-Matchmaker/lib/models"
	"crypto/rand"
	b64 "encoding/base64"
	"encoding/json"
	"errors"
	"fmt"
	"io/ioutil"
	"net/http"
	"strings"
	"time"

	"github.com/codegangsta/negroni"
	"github.com/gorilla/handlers"
	"github.com/gorilla/mux"
)

const (
	clientID       = "475b4f5384194becb712ad90112be788"
	clientSecretID = "24a4f437435345a09230cb7300a12d05"
	redirect_URI   = "http://localhost:8182/callback"
	respType       = "code"
	scopes         = "user-top-read"
	grant_type     = "authorization_code"
)

var (
	key = ""
)

var accseToke *string

type ObjectKey struct {
	Token string `json:"token"`
}

func main() {
	n := negroni.Classic()
	r := mux.NewRouter()
	r.Methods("GET").Path("/music").HandlerFunc(serveSpotify)
	r.Methods("GET").Path("/callback").HandlerFunc(callback)
	r.Methods("GET").Path("/music/api").HandlerFunc(handlerObject)
	// Serve the static files and templates from the static directory
	n.UseHandler(r)
	http.ListenAndServe(":8182", handlers.CORS()(r))

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
		fmt.Fprintf(w, "Sorry but you did not authorize access. Try later. Error: %s", err)
	}
	spotifyServURI := fmt.Sprintf("https://accounts.spotify.com/api/token?grant_type=%s&code=%s&redirect_uri=%s", grant_type, key, redirect_URI)
	request, err := http.NewRequest("POST", spotifyServURI, nil)
	if err != nil {
		panic("Error exchaging tokens. Please try later")
	}
	stirng64Base := fmt.Sprintf("%s:%s", clientID, clientSecretID)
	fmt.Println(stirng64Base)
	base64Key := b64.StdEncoding.EncodeToString([]byte(stirng64Base))
	strings.TrimSuffix(base64Key, "")
	fmt.Println(base64Key)
	strEnc := fmt.Sprintf(base64Key)
	newStrenc := strings.Replace(strEnc, "=", "", -1)

	fmt.Println(newStrenc)
	request.Header.Set("Authorization", "Basic "+newStrenc)
	request.Header.Set("Accept", " application/json")
	request.Header.Set("Content-Type", "application/x-www-form-urlencoded")

	//Authorization: Basic *<base64 encoded client_id:client_secret>*
	client := http.Client{}
	res, err := client.Do(request)
	responseObject := models.Reponse{}
	if err != nil {
		panic("token was not obtained")
	}
	defer res.Body.Close()
	data, err := ioutil.ReadAll(res.Body)
	if err != nil {
		fmt.Println(err)
	}
	d := string(data)
	fmt.Println(d)
	err = json.Unmarshal([]byte(d), &responseObject)
	if err != nil {
		fmt.Println("Error decoding json")
	}
	err = json.NewEncoder(w).Encode(responseObject)
	if err != nil {
		fmt.Println("Error decoding json", err)
	}
}

func handlerObject(w http.ResponseWriter, r *http.Request) {
	object := ObjectKey{
		Token: *accseToke,
	}
	err := json.NewEncoder(w).Encode(&object)
	if err != nil {
		fmt.Println("An error happened", err)
	}
}

func generateRandomString(upstream chan int, numb int) {
	timer := time.After(60)

	select {
	case <-timer:

	}
}

func autoGenerateBytes(n int, downstream chan []byte) {
	b := make([]byte, n)
	_, err := rand.Read(b)
	if err != nil {
		downstream <- nil
	}
	downstream <- b
}

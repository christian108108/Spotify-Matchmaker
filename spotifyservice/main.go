package main

import (
	b64 "encoding/base64"
	"encoding/json"
	"errors"
	"fmt"
	"io/ioutil"
	"net/http"
	"strings"

	"github.com/gorilla/mux"
)

const (
	clientID       = "475b4f5384194becb712ad90112be788"
	clientSecretID = "1aae388815834169a26f4d5e04259101"
	redirect_URI   = "http://localhost:8182/callback"
	respType       = "code"
	scopes         = "user-top-read"
	grant_type     = "authorization_code"
)

var (
	key = ""
)

var accseToke *string

type Reponse struct {
	access_token  string `json:"access_token"`
	token_type    string `json:"token_type"`
	scope         string `json:"scope"`
	expires_in    int    `json:"expires_in"`
	refresh_token string `json:"refresh_token"`
}

type ObjectKey struct {
	Token string `json:"token"`
}

func main() {
	r := mux.NewRouter()
	r.Methods("GET").Path("/music").HandlerFunc(serveSpotify)
	r.Methods("GET").Path("/callback").HandlerFunc(callback)
	r.Methods("GET").Path("/music/api").HandlerFunc(handlerObject)
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
	request.Header.Set("Content-Type", "application/x-www-form-urlencoded")
	//Authorization: Basic *<base64 encoded client_id:client_secret>*
	client := http.Client{}
	res, err := client.Do(request)
	if err != nil {
		panic("token was not obtained")
	}
	res.Header.Get("200")
	defer res.Body.Close()
	data, _ := ioutil.ReadAll(res.Body)
	dt := string(data)
	fmt.Println(dt)
	// Object2 := &Reponse{}
	// err = json.Unmarshal(data, Object2)
	// fmt.Println(Object2.access_token)
}

func handlerObject(w http.ResponseWriter, r *http.Request) {
	object := ObjectKey{
		Token: *accseToke,
	}
	err := json.NewEncoder(w).Encode(&object)
	if err != nil {
		fmt.Println("An error happened")
	}
}
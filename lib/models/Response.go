package models

type Reponse struct {
	Accesstoken  string `json:"access_token"`
	Token_type   string `json:"token_type"`
	Scope        string `json:"scope"`
	Expiresin    int    `json:"expires_in"`
	Refreshtoken string `json:"refresh_token"`
	PartyAccess  PartyCode
}

type PartyCode struct {
	Code string `json:"code"`
}

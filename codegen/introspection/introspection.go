package introspection

import "encoding/json"

func FromJson(introspectionJson string) (*Schema, error) {
	var resp Response
	if err := json.Unmarshal([]byte(introspectionJson), &resp); err != nil {
		return nil, err
	}
	return resp.Schema, nil
}

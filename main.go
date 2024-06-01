// The DotNet SDK and runtime.
package main

import (
	"context"
	_ "embed"
	"encoding/json"

	"github.com/Khan/genqlient/graphql"
)

//go:embed introspection.graphql
var introspectionQuery string

var introspectionPath = "/introspection.json"

type DotnetSdk struct{}

// Fetch introspection json from the Engine.
//
// This function forked from https://github.com/helderco/daggerverse/blob/main/codegen/main.go but
// didn't modify anything in the data.
//
// It's uses for test for the codegen only.
func (m *DotnetSdk) Introspect(ctx context.Context) (*File, error) {
	var resp json.RawMessage
	err := dag.GraphQLClient().MakeRequest(ctx, &graphql.Request{
		Query:  introspectionQuery,
		OpName: "IntrospectionQuery",
	}, &graphql.Response{
		Data: &resp,
	})
	if err != nil {
		return nil, err
	}
	return dag.
		Container().
		WithNewFile(introspectionPath, ContainerWithNewFileOpts{
			Contents: string(resp),
		}).
		File(introspectionPath), nil
}

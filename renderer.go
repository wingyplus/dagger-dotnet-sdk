package main

import (
	"context"
	"fmt"
	"strings"
)

type Enum struct {
	Name        string
	Description string
	Values      []string
}

type EnumRenderer struct{}

func (r *EnumRenderer) Render(enum *Enum) string {
	builder := &strings.Builder{}
	RenderDoc(builder, enum.Description)
	nl(builder)
	builder.WriteString(fmt.Sprintf("public enum %s {", enum.Name))
	nl(builder)
	for _, v := range enum.Values {
		indent(builder)
		builder.WriteString(v)
		builder.WriteRune(',')
		nl(builder)
	}
	nl(builder)
	builder.WriteRune('}')
	return builder.String()
}

func (r *EnumRenderer) RenderMany(ctx context.Context, enums []CodegenType) ([]string, error) {
	contents := make([]string, len(enums))
	for i, enum := range enums {
		e, err := r.IntoEnum(ctx, enum)
		if err != nil {
			return nil, err
		}
		contents[i] = r.Render(e)
	}
	return contents, nil
}

func (r *EnumRenderer) IntoEnum(ctx context.Context, enum CodegenType) (*Enum, error) {
	name, err := enum.Name(ctx)
	if err != nil {
		return nil, err
	}
	desc, err := enum.Description(ctx)
	if err != nil {
		return nil, err
	}
	enumValues, err := enum.EnumValues(ctx)
	if err != nil {
		return nil, err
	}

	e := &Enum{
		Name:        name,
		Description: desc,
		Values:      make([]string, len(enumValues)),
	}

	for i, ev := range enumValues {
		name, err := ev.Name(ctx)
		if err != nil {
			return nil, err
		}
		e.Values[i] = name
	}

	return e, nil
}

// type RenderInputValue struct{}
//
// func (r *RenderInputValue) RenderMany(inputs []CodegenType) {}
//
// RenderDoc render the code documentation.
func RenderDoc(builder *strings.Builder, description string) {
	lines := strings.Split(description, "\n")
	for i, line := range lines {
		lines[i] = "/// " + line
	}
	builder.WriteString("/// <summary>")
	nl(builder)
	builder.WriteString(strings.Join(lines, "\n"))
	nl(builder)
	builder.WriteString("/// </summary>")
}

func nl(builder *strings.Builder) {
	builder.WriteRune('\n')
}

func indent(builder *strings.Builder) {
	builder.WriteString(strings.Repeat(" ", 4))
}

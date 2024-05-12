package main

import "strings"

// TODO: render deprecated document.

// RenderDoc2 render the code documentation.
func RenderDoc2(builder *strings.Builder, description string) {
	RenderDoc3(builder, description, 0)
}

// RenderDoc3 similar to RenderDoc2 but allow to custom indentation.
func RenderDoc3(builder *strings.Builder, description string, nested int) {
	lines := strings.Split(description, "\n")
	for i, line := range lines {
		lines[i] = indent(nested) + "/// " + line
	}
	builder.WriteString(indent(nested) + "/// <summary>")
	nl(builder)
	builder.WriteString(strings.Join(lines, "\n"))
	nl(builder)
	builder.WriteString(indent(nested) + "/// </summary>")
}

// RenderIndent1 write an indentation into builder.
func RenderIndent1(builder *strings.Builder) {
	RenderIndent2(builder, 0)
}

// RenderIndent2 write an indentation into builder.
func RenderIndent2(builder *strings.Builder, nested int) {
	builder.WriteString(indent(nested))
}

// Write a new line into builder.
func nl(builder *strings.Builder) {
	builder.WriteRune('\n')
}

func indent(nested int) string {
	return strings.Repeat(" ", 4*nested)
}

func normalizeType(name string) string {
	switch name {
	case "String":
		return "string"
	case "Int":
		return "int"
	case "Float":
		return "float"
	default:
		return name
	}
}

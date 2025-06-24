import tempfile
import webbrowser
import svgwrite

class Charts:
    __type__ = "Charts"

    def generate_line_chart_svg(self, title, x_vals, y_vals, width=800, height=600):
        # Create a new SVG drawing with the given dimensions
        dwg = svgwrite.Drawing(size=(width, height))
        margin = 50  # Margin for axes and padding
        plot_width = width - 2 * margin
        plot_height = height - 2 * margin

        # Draw Y axis (vertical line)
        dwg.add(dwg.line(start=(margin, margin), end=(margin, height - margin), stroke='black'))
        # Draw X axis (horizontal line)
        dwg.add(dwg.line(start=(margin, height - margin), end=(width - margin, height - margin), stroke='black'))

        # Calculate scaling factors based on data range
        max_x = max(x_vals)
        min_x = min(x_vals)
        max_y = max(y_vals)
        min_y = min(y_vals)
        scale_x = plot_width / (max_x - min_x if max_x != min_x else 1)
        scale_y = plot_height / (max_y - min_y if max_y != min_y else 1)

        # Transform function to convert data coordinates into SVG canvas coordinates
        def transform(x, y):
            tx = margin + (x - min_x) * scale_x
            # Y values are inverted in SVG (0 at top), so we subtract from height
            ty = height - margin - (y - min_y) * scale_y
            return (tx, ty)

        # Convert all data points to SVG coordinates
        points = [transform(x, y) for x, y in zip(x_vals, y_vals)]

        # Draw the line connecting the points
        dwg.add(dwg.polyline(points=points, fill='none', stroke='black', stroke_width=2))

        # Draw circles at each data point
        for point in points:
            dwg.add(dwg.circle(center=point, r=3, fill='red'))

        # Add chart title centered at the top
        dwg.add(dwg.text(title, insert=(width / 2, margin / 2), text_anchor="middle", font_size=20))

        # Return the SVG content as a string
        return dwg.tostring()
    

def preview_svg_in_browser(svg_content):
    # Create a temporary HTML file to embed the SVG into
    with tempfile.NamedTemporaryFile(delete=False, suffix=".html", mode="w", encoding="utf-8") as f:
        # Construct simple HTML structure with embedded SVG
        html = f"""<!DOCTYPE html>
                <html>
                <head><meta charset="utf-8"><title>SVG Preview</title></head>
                <body>
                {svg_content}
                </body>
                </html>"""
        f.write(html)
        file_path = f.name  # Save the path to open later

    # Open the HTML file in the default web browser
    webbrowser.open(f"file://{file_path}")

# Example usage when the script is run directly
if __name__ == "__main__":
    charts = Charts()
    # Generate a simple line chart SVG from sample data
    svg = charts.generate_line_chart_svg("Demo Chart", [1, 2, 3, 4], [10, 5, 30, 15])
    # Preview the SVG in the default web browser
    preview_svg_in_browser(svg)

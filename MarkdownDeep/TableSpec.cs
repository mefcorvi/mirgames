// 
//   MarkdownDeep - http://www.toptensoftware.com/markdowndeep
//	 Copyright (C) 2010-2011 Topten Software
// 
//   Licensed under the Apache License, Version 2.0 (the "License"); you may not use this product except in 
//   compliance with the License. You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software distributed under the License is 
//   distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
//   See the License for the specific language governing permissions and limitations under the License.
//

using System.Collections.Generic;
using System.Text;

namespace Xilium.MarkdownDeep
{


	internal class TableSpec
	{
		public TableSpec()
		{
		}

		public bool LeadingBar;
		public bool TrailingBar;

		public List<TableCellAlignment> Columns = new List<TableCellAlignment>();

		public List<List<TableCellDefinition>> Headers = new List<List<TableCellDefinition>>();
		public List<List<TableCellDefinition>> Rows = new List<List<TableCellDefinition>>();

		public List<TableCellDefinition> ParseRow(StringScanner p)
		{
			p.SkipLinespace();

			if (p.eol)
				return null;		// Blank line ends the table

			bool bAnyBars = LeadingBar;
			if (LeadingBar && !p.SkipChar('|'))
			{
				return null;
			}

			// Create the row
			List<TableCellDefinition> row = new List<TableCellDefinition>();

			// Parse all columns except the last
			int totalColSpan = 0;

			while (!p.eol)
			{
				// Build new TableCellDefinition

				var cell = new TableCellDefinition();
				bool bAlignLeft = false;
				bool bAlignRight = false;

				// Check custom cell alignment
				bAlignLeft = p.SkipChar(':');

				// Check custom cell style
				if (p.SkipString("# ")) 
					cell.CellStyle = TableCellStyle.TH;

				// Find the next vertical bar
				p.Mark();
				while (!p.eol && p.current != '|')
					p.SkipForward(1);

				// Check custom cell alignment
				bAlignRight = p.DoesMatch(-2, " :");

				// Get cell content
				cell.Content = p.Extract(0, (bAlignRight ? -2 : 0)).Trim();

				// Get colspan
				bAnyBars |= p.SkipChar('|');
				int colSpan = 1;
				while (p.SkipChar('|')) colSpan++;
				cell.ColSpan = colSpan;
				totalColSpan += colSpan;

				// Set cell alignment
				if (bAlignLeft && bAlignRight)
					cell.Alignment = TableCellAlignment.Center;
				else if (bAlignLeft)
					cell.Alignment = TableCellAlignment.Left;
				else if (bAlignRight)
					cell.Alignment = TableCellAlignment.Right;

				// Add cell to row
				row.Add(cell);
			}

			// Require at least one bar to continue the table
			if (!bAnyBars)
				return null;

			// Add missing columns in Columns
			while (Columns.Count < totalColSpan) 
				Columns.Add(TableCellAlignment.NA);

			p.SkipEol();
			return row;
		}

		internal void RenderRow(Markdown m, StringBuilder b, List<TableCellDefinition> row, string type)
		{
			// Count of columns spanned
			var totalColSpan = 0;
			for (int i = 0; i < row.Count; i++)
				totalColSpan += row[i].ColSpan;

			// Add missing columns in row
			for (int i = totalColSpan; i < Columns.Count; i++)
				row.Add(new TableCellDefinition("&nbsp;", TableCellAlignment.NA, 1, 1, TableCellStyle.TD));

			// Render row
			for (int i = 0; i < row.Count; i++)
			{
				var alig = TableCellAlignment.NA;
				if (i < Columns.Count && Columns[i] != TableCellAlignment.NA) alig = Columns[i];

				var cell = row[i];
				b.Append("\t");
				cell.RenderOpenTag(b, type, alig);
				m.SpanFormatter.Format(b, cell.Content);
				cell.RenderCloseTag(b, type);
				b.Append("\n");
			}
		}
	
		public void Render(Markdown m, StringBuilder b)
		{
			b.Append("<table>\n");
			if (Headers != null && Headers.Count > 0)
			{
				b.Append("<thead>\n");
				foreach (var headerRow in Headers) {
					b.Append("<tr>\n");
					RenderRow(m, b, headerRow, "th");
					b.Append("</tr>\n");
				}
				b.Append("</thead>\n");
			}

			b.Append("<tbody>\n");
			foreach (var contentRow in Rows)
			{
				b.Append("<tr>\n");
				RenderRow(m, b, contentRow, null);
				b.Append("</tr>\n");
			}
			b.Append("</tbody>\n");

			b.Append("</table>\n");
		}

		public static TableSpec Parse(StringScanner p)
		{
			// Leading line space allowed
			p.SkipLinespace();

			// Quick check for typical case
			if (p.current != '|' && p.current != ':' && p.current != '-')
				return null;

			// Don't create the spec until it at least looks like one
			TableSpec spec = null;

			// Leading bar, looks like a table spec
			if (p.SkipChar('|'))
			{
				spec=new TableSpec();
				spec.LeadingBar=true;
			}


			// Process all columns
			while (true)
			{
				// Parse column spec
				p.SkipLinespace();

				// Must have something in the spec
				if (p.current == '|')
					return null;

				bool AlignLeft = p.SkipChar(':');
				while (p.current == '-')
					p.SkipForward(1);
				bool AlignRight = p.SkipChar(':');
				p.SkipLinespace();

				// Work out column alignment
				TableCellAlignment col = TableCellAlignment.NA;
				if (AlignLeft && AlignRight)
					col = TableCellAlignment.Center;
				else if (AlignLeft)
					col = TableCellAlignment.Left;
				else if (AlignRight)
					col = TableCellAlignment.Right;

				if (p.eol)
				{
					// Not a spec?
					if (spec == null)
						return null;

					// Add the final spec?
					spec.Columns.Add(col);
					return spec;
				}

				// We expect a vertical bar
				if (!p.SkipChar('|'))
					return null;

				// Create the table spec
				if (spec==null)
					spec=new TableSpec();

				// Add the column
				spec.Columns.Add(col);

				// Check for trailing vertical bar
				p.SkipLinespace();
				if (p.eol)
				{
					spec.TrailingBar = true;
					return spec;
				}

				// Next column
			}
		}
	}
}

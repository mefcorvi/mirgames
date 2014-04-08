// 
//   MarkdownDeep - http://www.xilium.it/
//	 Copyright (C) 2013 Xilium Software
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

using System.Text;

namespace Xilium.MarkdownDeep {


	internal enum TableCellAlignment {
		NA,
		Left,
		Right,
		Center,
	}


	internal enum TableCellStyle {
		TD		// normal table cell
		, TH	// head table cell
	}


	internal class TableCellDefinition {
		private string _content;
		private TableCellAlignment _alignment;
		private int _colSpan;
		private int _rowSpan;
		private TableCellStyle _cellStyle;

		public TableCellDefinition() : this(null, TableCellAlignment.NA, 1, 1, TableCellStyle.TD) {
			
		}
		public TableCellDefinition(string content, TableCellAlignment alignment, int colSpan, int rowSpan, TableCellStyle cellStyle) {
			this._content = content;
			this._alignment = alignment;
			this._colSpan = colSpan;
			this._rowSpan = rowSpan;
			this._cellStyle = cellStyle;
		}

		public string Content {
			get { return this._content; }
			set { this._content = value; }
		}

		public TableCellAlignment Alignment {
			get { return this._alignment; }
			set { this._alignment = value; }
		}

		public int ColSpan {
			get { return this._colSpan; }
			set { if (value > 0) this._colSpan = value; }
		}

		public int RowSpan {
			get { return this._rowSpan; }
			set { if (value > 0) this._rowSpan = value; }
		}

		public TableCellStyle CellStyle {
			get { return this._cellStyle; }
			set { this._cellStyle = value; }
		}

		public string TagName {
			get {
				switch (this._cellStyle) {
					case TableCellStyle.TH:
						return "th";
					case TableCellStyle.TD:
						return "td";
				}
				return null;
			}
		}

		public void RenderOpenTag(StringBuilder b, string tagName, TableCellAlignment columnAlignment) {
			b.Append("<");

			// Add tagName (priority to argument)
			b.Append(tagName ?? this.TagName);

			// Determining alignment (priority to cell)
			var alig = (this.Alignment != TableCellAlignment.NA ? this.Alignment : columnAlignment);
			switch (alig) {
				case TableCellAlignment.Left:
					b.Append(" align=\"left\"");
					break;
				case TableCellAlignment.Right:
					b.Append(" align=\"right\"");
					break;
				case TableCellAlignment.Center:
					b.Append(" align=\"center\"");
					break;
			}

			if (this.ColSpan > 1) {
				b.Append(" colspan=\"");
				b.Append(this.ColSpan);
				b.Append("\"");
			}
			if (this.RowSpan > 1) {
				b.Append(" rowspan=\"");
				b.Append(this.RowSpan);
				b.Append("\"");
			}
			b.Append(">");
		}
		public void RenderCloseTag(StringBuilder b, string tagName) {
			b.Append("</");
			b.Append(tagName ?? this.TagName);
			b.Append(">");
		}
	}
}

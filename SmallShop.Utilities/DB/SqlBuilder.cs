using System;
using System.Text;

namespace SmallShop.Utilities
{
    /// <summary>
    /// 表示SQL构造器。
    /// </summary>
    public class SqlBuilder
    {
        #region Declarations

        private int _groupCount = 0;
        private bool firstAppendedField = true;//是否是首次追加字段
        private StringBuilder sb = new StringBuilder();

        #endregion Declarations

        #region Constructors

        public SqlBuilder() {
            junction = SqlUtil.AND;
            ignoreCase = false;
        }

        public SqlBuilder(bool ignoreCase) {
            junction = SqlUtil.AND;
            this.ignoreCase = ignoreCase;
        }

        public SqlBuilder(bool ignoreCase, bool useAnd) {
            junction = useAnd ? SqlUtil.AND : SqlUtil.OR;
            this.ignoreCase = ignoreCase;
        }

        public static SqlBuilder New {
            get {
                return new SqlBuilder();
            }
        }

        #endregion Constructors

        #region Append

        public virtual SqlBuilder Append(string sql) {
            sb.Append(sql);

            return this;
        }

        public virtual SqlBuilder Append(string sql, bool withSpace) {
            if (withSpace)
                sb.Append(" " + sql + " ");
            else
                sb.Append(sql);

            return this;
        }

        #endregion Append

        #region AppendField

        public virtual SqlBuilder AppendField(string expression, String alias) {
            if (firstAppendedField) {
                if (expression.Trim() == "*")
                    sb.Append(" * ");
                else
                    sb.Append(string.Format(" {0} as {1} ", expression, alias));
                firstAppendedField = false;
            } else {
                if (expression.Trim() == "*")
                    sb.Append(" ,* ");
                else
                    sb.Append(string.Format(" , {0} as {1} ", expression, alias));
            }

            return this;
        }

        public virtual SqlBuilder AppendField(string expression) {
            if (firstAppendedField) {
                if (expression.Trim() == "*")
                    sb.Append(" * ");
                else
                    sb.Append(expression);
                firstAppendedField = false;
            } else {
                if (expression.Trim() == "*")
                    sb.Append(" ,* ");
                else
                    sb.Append(string.Format(" , {0} ", expression));
            }

            return this;
        }

        #endregion

        #region AppendEquals

        public virtual SqlBuilder AppendEquals(object column, object value) {
            return AppendEquals(junction, column, value, false);
        }

        public virtual SqlBuilder AppendEquals(object column, bool value) {
            return AppendEquals(junction, column, value ? "1" : "0", false);
        }

        public virtual SqlBuilder AppendEquals(object column, String value, bool surround) {
            return AppendEquals(junction, column, value, surround);
        }

        public virtual SqlBuilder AppendEquals(object column, object value, bool surround) {
            return AppendEquals(junction, column, value, surround);
        }

        public virtual SqlBuilder AppendEquals(String junction, object column, object value, bool surround) {
            if (!String.IsNullOrEmpty(value.ToString())) {
                value = surround ? "'" + value.ToString() + "'" : value.ToString();
                AppendInternal(junction, column, "=", value.ToString());
            }

            return this;
        }

        #endregion AppendEquals


        #region AppendLike
        public virtual SqlBuilder AppendLike(object column, String value, bool surround) {
            return AppendLike(junction, column, value, surround);
        }

        public virtual SqlBuilder AppendLike(String junction, object column, String value, bool surround) {
            if (!String.IsNullOrEmpty(value)) {
                value = surround ? "'%" + value + "%'" : "'%" + value + "%'";
                AppendInternal(junction, column, "like", value);
            }

            return this;
        }

        /// <summary>
        /// Gets the like format string.
        /// </summary>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <param name="surround"></param>
        /// <returns></returns>
        private String GetLikeFormat(bool ignoreCase, bool surround) {
            if (surround) {
                return ignoreCase ? "UPPER({0}) LIKE UPPER('{1}')" : "{0} LIKE '{1}'";
            }

            return ignoreCase ? "UPPER({0}) LIKE UPPER({1})" : "{0} LIKE {1}";
        }
        #endregion AppendLike

        #region AppendNotEquals

        public virtual SqlBuilder AppendNotEquals(object column, String value, bool surround) {
            return AppendNotEquals(junction, column, value, surround);
        }

        public virtual SqlBuilder AppendNotEquals(String junction, object column, String value, bool surround) {
            if (!String.IsNullOrEmpty(value)) {
                value = surround ? "'" + value + "'" : value;
                AppendInternal(junction, column, "<>", value);
            }

            return this;
        }

        #endregion AppendNotEquals

        #region AppendIn

        public virtual SqlBuilder AppendIn(object column, String values) {
            return AppendIn(junction, column, values);
        }

        public virtual SqlBuilder AppendIn(String junction, object column, String values) {
            if (!String.IsNullOrEmpty(values)) {
                values = GetInQueryValues(values);

                if (!String.IsNullOrEmpty(values)) {
                    AppendInQuery(junction, column, values);
                }
            }

            return this;
        }

        #endregion AppendIn

        #region AppendNotIn

        public virtual SqlBuilder AppendNotIn(object column, String values) {
            return AppendNotIn(junction, column, values);
        }

        public virtual SqlBuilder AppendNotIn(String junction, object column, String values) {
            if (!String.IsNullOrEmpty(values)) {
                values = GetInQueryValues(values);

                if (!String.IsNullOrEmpty(values)) {
                    AppendNotInQuery(junction, column, values);
                }
            }

            return this;
        }

        #endregion AppendNotIn

        #region AppendInQuery

        public virtual SqlBuilder AppendInQuery(object column, String query) {
            return AppendInQuery(junction, column, query);
        }

        public virtual SqlBuilder AppendInQuery(String junction, object column, String query) {
            if (!String.IsNullOrEmpty(query)) {
                AppendInternal(junction, column, "IN", "(" + query + ")");
            }

            return this;
        }

        #endregion AppendInQuery

        #region AppendNotInQuery

        public virtual SqlBuilder AppendNotInQuery(object column, String query) {
            return AppendNotInQuery(junction, column, query);
        }

        public virtual SqlBuilder AppendNotInQuery(String junction, object column, String query) {
            if (!String.IsNullOrEmpty(query)) {
                AppendInternal(junction, column, "NOT IN", "(" + query + ")");
            }

            return this;
        }

        #endregion AppendNotInQuery

        #region AppendRange

        public virtual SqlBuilder AppendRange(object column, String from, String to, bool surround) {
            return AppendRange(junction, column, from, to, surround);
        }

        public virtual SqlBuilder AppendRange(String junction, object column, String from, String to, bool surround) {

            if (!String.IsNullOrEmpty(from) || !String.IsNullOrEmpty(to)) {
                StringBuilder sb = new StringBuilder();

                if (!String.IsNullOrEmpty(from)) {
                    sb.AppendFormat("{0} >= {1}", column, surround ? "'" + from + "'" : from);
                }
                if (!String.IsNullOrEmpty(from) && !String.IsNullOrEmpty(to)) {
                    sb.AppendFormat(" {0} ", SqlUtil.AND);
                }
                if (!String.IsNullOrEmpty(to)) {
                    sb.AppendFormat("{0} <= {1}", column, surround ? "'" + to + "'" : to);
                }

                AppendInternal(junction, sb.ToString());
            }

            return this;
        }

        #endregion AppendRange

        #region AppendIsNull

        public virtual SqlBuilder AppendIsNull(object column) {
            return AppendIsNull(junction, column);
        }

        public virtual SqlBuilder AppendIsNull(String junction, object column) {
            AppendInternal(junction, SqlUtil.IsNull(column));
            return this;
        }

        #endregion AppendIsNull

        #region AppendIsNotNull

        public virtual SqlBuilder AppendIsNotNull(object column) {
            return AppendIsNotNull(junction, column);
        }

        public virtual SqlBuilder AppendIsNotNull(String junction, object column) {
            AppendInternal(junction, SqlUtil.IsNotNull(column));
            return this;
        }

        #endregion AppendIsNotNull

        #region AppendGreaterThan

        public virtual SqlBuilder AppendGreaterThan(object column, String value, bool surround) {
            return AppendGreaterThan(junction, column, value, surround);
        }

        public virtual SqlBuilder AppendGreaterThan(String junction, object column, String value, bool surround) {
            if (!String.IsNullOrEmpty(value)) {
                AppendInternal(junction, column, ">", surround ? "'" + value + "'" : value);
            }

            return this;
        }

        #endregion AppendGreaterThan

        #region AppendGreaterThanOrEqual

        public virtual SqlBuilder AppendGreaterThanOrEqual(object column, String value, bool surround) {
            return AppendGreaterThanOrEqual(junction, column, value, surround);
        }

        public virtual SqlBuilder AppendGreaterThanOrEqual(String junction, object column, String value, bool surround) {
            if (!String.IsNullOrEmpty(value)) {
                AppendInternal(junction, column, ">=", surround ? "'" + value + "'" : value);
            }

            return this;
        }

        #endregion AppendGreaterThanOrEqual

        #region AppendLessThan

        public virtual SqlBuilder AppendLessThan(object column, String value, bool surround) {
            return AppendLessThan(junction, column, value, surround);
        }

        public virtual SqlBuilder AppendLessThan(String junction, object column, String value, bool surround) {
            if (!String.IsNullOrEmpty(value)) {
                AppendInternal(junction, column, "<", surround ? "'" + value + "'" : value);
            }

            return this;
        }

        #endregion AppendLessThan

        #region AppendLessThanOrEqual

        public virtual SqlBuilder AppendLessThanOrEqual(object column, String value, bool surround) {
            return AppendLessThanOrEqual(junction, column, value, surround);
        }

        public virtual SqlBuilder AppendLessThanOrEqual(String junction, object column, String value, bool surround) {
            if (!String.IsNullOrEmpty(value)) {
                AppendInternal(junction, column, "<=", surround ? "'" + value + "'" : value);
            }

            return this;
        }

        #endregion AppendLessThanOrEqual

        #region AppendInternal

        protected virtual void AppendInternal(String junction, object column, String compare, String value) {
            AppendInternal(junction, String.Format("{0} {1} {2}", column, compare, value));
        }

        protected virtual void AppendInternal(String junction, String query) {
            if (!String.IsNullOrEmpty(query)) {
#if DEBUG
                String end = Environment.NewLine;
#else
				String end = String.Empty;
#endif
                String format = (sb.Length > 0) ? " {0} ({1}){2}" : " ({1}){2}";
                sb.AppendFormat(format, junction, query, end);
            }
        }

        #endregion AppendInternal

        #region Methods

        public virtual void Clear() {
            sb.Length = 0;
            _groupCount = 0;
        }

        public override string ToString() {
            return sb.ToString().TrimEnd();
        }

        public virtual string ToString(String junction) {
            if (sb.Length > 0) {
                return new StringBuilder(" ").Append(junction).Append(" ").Append(ToString()).ToString();
            }

            return String.Empty;
        }

        public virtual String GetInQueryValues(String values) {
            String[] split = values.Split(',');
            values = SqlUtil.Encode(split);

            return values;
        }

        public virtual void BeginGroup() {
            BeginGroup(junction);
        }

        public virtual void BeginGroup(string junction) {
            sb.AppendFormat("{0} (", junction);
            _groupCount++;
        }

        public virtual void EndGroup() {
            if (_groupCount > 0) {
                sb.Append(")");
                _groupCount--;
            }
        }

        internal virtual void EnsureGroups() {
            while (_groupCount > 0) {
                EndGroup();
            }
        }

        #endregion Methods

        #region Properties

        private bool ignoreCase;

        private String junction;

        public virtual int Length {
            get { return sb.Length; }
        }

        #endregion Properties
    }
}
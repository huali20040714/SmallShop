using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SmallShop.Utilities.Lib.AppLog
{
    /// <summary>
    /// IComparer，　
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    internal sealed class Reverser<T> : IComparer<T>
    {

        private readonly Type _type = null;
        private ReverserInfo _info;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="direction">(/)</param>
        public Reverser(Type type, string name, ReverserInfo.Direction direction)
        {
            this._type = type;
            this._info.name = name;
            if (direction != ReverserInfo.Direction.ASC)
                this._info.direction = direction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <param name="name"></param>
        /// <param name="direction">(/)</param>
        public Reverser(string className, string name, ReverserInfo.Direction direction)
        {
            try
            {
                this._type = Type.GetType(className, true);
                this._info.name = name;
                this._info.direction = direction;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="name"></param>
        /// <param name="direction">(/)</param>
        public Reverser(T t, string name, ReverserInfo.Direction direction)
        {
            this._type = t.GetType();
            this._info.name = name;
            this._info.direction = direction;
        }

        //！IComparer<T>。
        int IComparer<T>.Compare(T t1, T t2)
        {
            object x = this._type.InvokeMember(this._info.name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, t1, null);
            object y = this._type.InvokeMember(this._info.name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, t2, null);
            if (this._info.direction != ReverserInfo.Direction.ASC)
                Swap(ref x, ref y);
            return (new CaseInsensitiveComparer()).Compare(x, y);
        }

        //
        private void Swap(ref object x, ref object y)
        {
            object temp = null;
            temp = x;
            x = y;
            y = temp;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public struct ReverserInfo
    {

        /// <summary>
        /// ，：
        /// ASC：
        /// DESC：
        /// </summary>
        public enum Direction
        {
            /// <summary>
            /// 
            /// </summary>
            ASC = 0,
            /// <summary>
            /// 
            /// </summary>
            DESC = 1,
        };

        /// <summary>
        /// 
        /// </summary>
        public enum Target
        {
            /// <summary>
            /// 
            /// </summary>
            CUSTOMER = 0,
            /// <summary>
            /// 
            /// </summary>
            FORM,
            /// <summary>
            /// 
            /// </summary>
            FIELD,
            /// <summary>
            /// 
            /// </summary>
            SERVER,
        };

        /// <summary>
        /// 
        /// </summary>
        public string name;
        /// <summary>
        /// 
        /// </summary>
        public Direction direction;
        /// <summary>
        /// 
        /// </summary>
        public Target target;
    }
}

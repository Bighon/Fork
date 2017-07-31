﻿//************************************************************************
//      https://github.com/yuzhengyang
//      author:     yuzhengyang
//      date:       2017.7.31 - 2017.7.31
//      desc:       窗体管理器
//      Copyright (c) yuzhengyang. All rights reserved.
//************************************************************************
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Y.Utils.WindowsUtils.FormUtils
{
    /// <summary>
    /// 窗体管理器
    /// </summary>
    public class FormManTool
    {
        protected ConcurrentDictionary<Type, Form> UniqueForms = new ConcurrentDictionary<Type, Form>();

        public List<Form> AllForms { get { return _AllForms; } }
        private List<Form> _AllForms = new List<Form>();

        /// <summary>
        /// 获取窗体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() where T : Form, new()
        {
            if (UniqueForms.ContainsKey(typeof(T)))
            {
                // 字典中有该窗体，则读取窗体对象
                Form value;
                if (UniqueForms.TryGetValue(typeof(T), out value))
                {
                    if (value != null && !value.IsDisposed)
                    {
                        // 窗体对象可用（不为空、没释放），反馈窗体对象
                        return (T)value;
                    }
                    else
                    {
                        // 窗体对象不可用，从字典中移除窗体对象
                        Form temp;
                        UniqueForms.TryRemove(typeof(T), out temp);
                    }
                }
            }

            // 未能返回正确的窗体，则创建新窗体（使用默认new方法）
            T form = new T();
            if (UniqueForms.TryAdd(typeof(T), form))
            {
                // 添加到字典成功后，返回当前窗体对象
                _AllForms.Add(form);
                return form;
            }
            return null;
        }
    }
}

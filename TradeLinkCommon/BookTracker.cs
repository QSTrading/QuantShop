﻿using System;
using System.Collections.Generic;
using System.Text;
using TradeLink.API;
using TradeLink.Common;

namespace TradeLink.Common
{
    /// <summary>
    /// helps create books from ticks and keep track of book events
    /// </summary>
    public class BookTracker : GotTickIndicator
    {
        public void GotTick(Tick k) { newTick(k); }
        bool _bizero = false;
        int _minbookwait = 5;

        Dictionary<string, bool> _start = new Dictionary<string, bool>();
        Dictionary<string, Book> _book = new Dictionary<string, Book>();
        /// <summary>
        /// obtain events when a new book is received
        /// </summary>
        public event DebugDelegate NewBook;

        /// <summary>
        /// retrieve present book for current symbol.
        /// not thread safe.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public Book this[string symbol]
        {
            get
            {
                Book b;
                if (_book.TryGetValue(symbol, out b))
                    return b;
                return new Book(symbol);
            }
            set
            {
                Book b;
                if (_book.TryGetValue(symbol, out b))
                    _book[symbol] = value;
                else
                    _book.Add(symbol, value);
            }
        }

        /// <summary>
        /// This allows you to control the # of seconds between book updates.
        /// This can help to conserve CPU load if you don't need to see every book.
        /// Set to 0 to see every book.
        /// </summary>
        public int BookInterval { get { return _minbookwait; } set { _minbookwait = value; _bizero = _minbookwait == 0; } }
        /// <summary>
        /// create books from tick updates
        /// </summary>
        /// <param name="k"></param>
        /// 
        bool _update = false;
        public void newTick(Tick k)
        {
            // make sure we update every few seconds
            if (!_bizero && ((k.time % _minbookwait) == 0))
            {
                _update = true;
            }
            bool NEWFINISHEDBOOK = false;
            // we don't need an update
            if (!_update) return;
            // we need an update, have we started?
            bool started = false;
            if (!_start.TryGetValue(k.symbol, out started))
                _start.Add(k.symbol, false);
            // haven't started and not good place to start
            if (!started && (k.depth != 0)) return;
            else if (!started) // ready to start
                _start[k.symbol] = true;
            else if (started && (k.depth == 0)) // done for now
            {
                // FINISHED FIRST BOOK IN THIS INTERVAL
                NEWFINISHEDBOOK = true;
                // reset flags
                _start[k.symbol] = false;
                _update = false;
            }
            // make sure book exists 
            Book b;
            if (!_book.TryGetValue(k.symbol, out b))
            {
                b = new Book(k.symbol);
                _book.Add(k.symbol, b);
            }
            // if we have new book, notify 
            if (NEWFINISHEDBOOK && (NewBook != null))
                NewBook(k.symbol);
            // pass tick
            b.GotTick(k);
            // save book
            _book[k.symbol] = b;
        }
    }
}

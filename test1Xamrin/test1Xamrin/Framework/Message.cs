﻿using System;
using System.Collections.Generic;
using System.Text;

namespace test1Xamrin.Framework
{
    public class Message
    {
        public string Text { get; set; }
        public MessageType MessageType { get; set; }
        public DateTime DateTime { get; set; }
        public string UserTime
        {
            get
            {
                switch (MessageType)
                {
                    case MessageType.Received:
                        return $"Bot at {DateTime.ToString("HH:mm:ss")}";
                    case MessageType.Sent:
                        return $"User at {DateTime.ToString("HH:mm:ss")}";
                }

                return string.Empty;

            }
        }
    }
}

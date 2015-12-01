/*
 * Encoding.cs - Implementation of the "System.Text.Encoding" class.
 *
 * Copyright (c) 2001, 2002  Southern Storm Software, Pty Ltd
 * Copyright (c) 2002, Ximian, Inc.
 * Copyright (c) 2003, 2004 Novell, Inc.
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */

using System;

namespace SyslogNet.Client.Text
{
    public abstract class Encoding : System.Text.Encoding
    {
        bool is_readonly = true;

        protected Encoding(int codePage)
        {
            switch (codePage)
            {
                default:
                    // MS has "InternalBestFit{Decoder|Encoder}Fallback
                    // here, but we dunno what they are for.
                    decoder_fallback = DecoderFallback.ReplacementFallback;
                    encoder_fallback = EncoderFallback.ReplacementFallback;
                    break;
                case 20127: // ASCII
                case 54936: // GB18030
                    decoder_fallback = DecoderFallback.ReplacementFallback;
                    encoder_fallback = EncoderFallback.ReplacementFallback;
                    break;
                case 1200: // UTF16
                case 1201: // UTF16
                case 12000: // UTF32
                case 12001: // UTF32
                case 65000: // UTF7
                case 65001: // UTF8
                    decoder_fallback = DecoderFallback.StandardSafeFallback;
                    encoder_fallback = EncoderFallback.StandardSafeFallback;
                    break;
            }
        }
        
        DecoderFallback decoder_fallback;
        EncoderFallback encoder_fallback;

        public bool IsReadOnly
        {
            get { return is_readonly; }
        }

        public DecoderFallback DecoderFallback
        {
            get { return decoder_fallback; }
            set
            {
                if (IsReadOnly)
                    throw new InvalidOperationException("This Encoding is readonly.");

                if (value == null)
                    throw new ArgumentNullException();

                decoder_fallback = value;
            }
        }

        public EncoderFallback EncoderFallback
        {
            get { return encoder_fallback; }
            set
            {
                if (IsReadOnly)
                    throw new InvalidOperationException("This Encoding is readonly.");

                if (value == null)
                    throw new ArgumentNullException();

                encoder_fallback = value;
            }
        }

        static volatile Encoding asciiEncoding;
        static readonly object lockobj = new object();

        // Get the standard ASCII encoding object.
        public static Encoding ASCII
        {
            get
            {
                if (asciiEncoding == null)
                {
                    lock (lockobj)
                    {
                        if (asciiEncoding == null)
                        {
                            asciiEncoding = new ASCIIEncoding();
                            //asciiEncoding.is_readonly = true;
                        }
                    }
                }

                return asciiEncoding;
            }
        }
    }
}

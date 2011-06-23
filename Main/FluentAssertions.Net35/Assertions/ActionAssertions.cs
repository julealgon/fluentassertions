﻿using System;
using System.Diagnostics;

namespace FluentAssertions.Assertions
{
    [DebuggerNonUserCode]
    public class ActionAssertions
    {
        protected internal ActionAssertions(Action subject)
        {
            Subject = subject;
        }

        public Action Subject { get; private set; }

        public ExceptionAssertions<TException> ShouldThrow<TException>(string reason, object[] reasonParameters)
            where TException : Exception
        {
            Exception exception = null;

            try
            {
                Subject();
            }
            catch (Exception actualException)
            {
                exception = actualException;
            }

            Execute.Verification
                .ForCondition(exception != null)
                .BecauseOf(reason, reasonParameters)
                .FailWith("Expected {1}{0}, but no exception was thrown.", typeof(TException));


            Execute.Verification
                .ForCondition(exception is TException)
                .BecauseOf(reason, reasonParameters)
                .FailWith("Expected {1}{0}, but found {2}.", typeof(TException), exception);

            return new ExceptionAssertions<TException>((TException)exception);            
        }

        public void ShouldNotThrow<TException>(string reason, object[] reasonArgs)
        {
            Exception exception = null;

            try
            {
                Subject();
            }
            catch (Exception actualException)
            {
                exception = actualException;
            }

            if (exception != null)
            {
                Execute.Verification
                    .ForCondition(!(exception is TException))
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Did not expect {1}{0}, but found one with message {2}.",
                        typeof (TException), exception.Message);
            }
        }

        public void ShouldNotThrow(string reason, object[] reasonParameters)
        {
            try
            {
                Subject();
            }
            catch (Exception exception)
            {
                Execute.Fail("Did not expect any exception{2}, but found a {0} with message {1}.",
                    exception.GetType(), exception.Message, reason, reasonParameters);
            }
        }
    }
}

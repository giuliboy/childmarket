using System;
using System.ComponentModel;
using System.Windows.Input;

namespace KinderArtikelBoerse.Utils
{
    public class ActionCommand : ICommand
    {
        private readonly Action m_action;
        public ActionCommand( Action action )
        {
            m_action = action;
        }

        public bool CanExecute( object parameter )
        {
            return true;
        }

        public void Execute( object parameter )
        {
            m_action();
        }

        public event EventHandler CanExecuteChanged;
    }

    public class ActionCommand<T> : ICommand
    {
        readonly Predicate<T> m_canExecute = null;

        public ActionCommand( Action<T> action, Predicate<T> canExecute = null )
        {
            m_action = action;
            m_canExecute = canExecute;
        }

        public bool CanExecute( object parameter )
        {
            return m_canExecute == null || m_canExecute( (T)parameter );
        }

        /// <inheritdoc />
        public virtual void Execute( object parameter )
        {
            T convertedParameter = ConvertParameter( parameter );
            try
            {
                m_action( convertedParameter );
            }
            catch ( Exception ex )
            {
                Console.Write( ex );
                throw;
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        protected readonly Action<T> m_action;

        /// <summary>
        /// Convert a given <paramref name="value"/> into type T.
        /// </summary>
        /// <param name="value">The value to be converted into type T.</param>
        /// <returns>The converted value (which may be default of T).</returns>
        protected virtual T ConvertParameter( object value )
        {
            T convertedParameter = default( T );
            if ( value != null )
            {
                var toType = typeof( T );
                var fromType = value.GetType();

                if ( toType.IsAssignableFrom( fromType ) /* type of value can be casted to type of  parameter */)
                {
                    convertedParameter = (T)value;
                }
                else
                {
                    var converter = TypeDescriptor.GetConverter( toType );

                    if ( converter != null && converter.CanConvertTo( toType ) && converter.CanConvertFrom( fromType ) )
                    {
                        convertedParameter = (T)converter.ConvertTo( value, toType );
                    }
                    else
                    {
                        string text = String.Format( "Parameter type '{0}' cannot be converted or casted to '{1}'", fromType, toType );
                    }
                }
            }
            return convertedParameter;
        }
    }
}
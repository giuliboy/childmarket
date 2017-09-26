using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace KinderArtikelBoerse.Utils
{
    /// <summary>
    /// Behavior that will connect an UI event to a viewmodel Command,
    /// allowing the event arguments to be passed as the CommandParameter.
    /// </summary>
    public class EventCommandBehavior : Behavior<FrameworkElement>
    {
        private Delegate m_handler;
        private EventInfo m_oldEvent;

        // Event
        public string Event { get { return (string)GetValue( EventProperty ); } set { SetValue( EventProperty, value ); } }
        public static readonly DependencyProperty EventProperty = DependencyProperty.Register( "Event", typeof( string ), typeof( EventCommandBehavior ), new PropertyMetadata( null, OnEventChanged ) );

        // Command
        public ICommand Command { get { return (ICommand)GetValue( CommandProperty ); } set { SetValue( CommandProperty, value ); } }
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register( "Command", typeof( ICommand ), typeof( EventCommandBehavior ), new PropertyMetadata( null ) );

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter", typeof( object ), typeof( EventCommandBehavior ), new PropertyMetadata( default( object ) ) );

        public object CommandParameter { get { return (object)GetValue( CommandParameterProperty ); } set { SetValue( CommandParameterProperty, value ); } }

        // PassArguments (default: false)
        public bool PassArguments { get { return (bool)GetValue( PassArgumentsProperty ); } set { SetValue( PassArgumentsProperty, value ); } }
        public static readonly DependencyProperty PassArgumentsProperty = DependencyProperty.Register( "PassArguments", typeof( bool ), typeof( EventCommandBehavior ), new PropertyMetadata( false ) );


        private static void OnEventChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var beh = (EventCommandBehavior)d;

            if ( beh.AssociatedObject != null ) // is not yet attached at initial load
            {
                beh.AttachHandler( (string)e.NewValue );
            }

        }

        protected override void OnAttached()
        {
            AttachHandler( Event ); // initial set
        }

        /// <summary>
        /// Attaches the handler to the event
        /// </summary>
        private void AttachHandler( string eventName )
        {
            // detach old event
            if ( m_oldEvent != null )
            {
                m_oldEvent.RemoveEventHandler( AssociatedObject, m_handler );
            }
            // attach new event
            if ( !string.IsNullOrEmpty( eventName ) )
            {
                EventInfo ei = AssociatedObject.GetType().GetEvent( eventName );
                if ( ei != null )
                {
                    MethodInfo mi = GetType().GetMethod( "ExecuteCommand", BindingFlags.Instance | BindingFlags.NonPublic );
                    m_handler = Delegate.CreateDelegate( ei.EventHandlerType, this, mi );
                    ei.AddEventHandler( AssociatedObject, m_handler );
                    m_oldEvent = ei; // store to detach in case the Event property changes
                }
                else
                {
                    throw new ArgumentException( string.Format( "The event '{0}' was not found on type '{1}'", eventName, AssociatedObject.GetType().Name ) );
                }

            }
        }

        /// <summary>
        /// Executes the Command
        /// </summary>
        private void ExecuteCommand( object sender, EventArgs e )
        {
            object parameter = CommandParameter ?? ( PassArguments ? e : null );
            if ( Command != null )
            {
                if ( Command.CanExecute( parameter ) )
                {
                    Command.Execute( parameter );
                }

            }
        }

    }
}
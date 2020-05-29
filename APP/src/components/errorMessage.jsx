import React from 'react';

class ErrorMessage extends React.Component{

    render(){
        return (
            <div className="s-field--danger">
                <h1>Błąd!</h1>
                {this.props.message}
            </div>
        );
    }

}

export default ErrorMessage;
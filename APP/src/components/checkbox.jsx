import React from 'react';

class Checkbox extends React.Component{

    state = {
        checked: false
    }

    check = ()=>{
        this.setState({
            checked: !this.state.checked
        });
        this.props.onCheck(!this.state.checked);
    }

    render(){
        return(
            <div className={`checkbox ${this.state.checked ? 'active' : ''}`} onClick={this.check}>
                {this.props.children} 
                <span></span>
            </div>
        );
    }

}

export default Checkbox;
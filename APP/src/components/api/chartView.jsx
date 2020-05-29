import React from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';
import InputRange from 'react-input-range';
import 'react-input-range/lib/css/index.css';

import Card from '../card.jsx';
import Loader from '../loader.jsx';
import APIComponent from './apiComponent.jsx';

class ChartView extends APIComponent{

    state = {
        load: false,
        n: 200
    }

    renewStateValues(){
        let variables = APIComponent.getVariables(this.props.equation);
        let xValue = (variables.x!==undefined) ? variables.x : 0;
        this.setState({
            from: xValue-10,
            to: xValue+10,
            fromTxt: (xValue-10).toString(),
            toTxt: (xValue+10).toString(),
            mounted: true,
            reload: true
        });
    }

    componentDidMount(prevProps,prevState){
        this.renewStateValues();
    }

    componentDidUpdate(prevProps,prevState){
        if(prevProps.equation!==this.props.equation)
            this.renewStateValues();
        if(prevState.reload!==this.state.reload)
            this.loadData('xy');
    }

    reload = ()=>{
        this.setState({reload: true});
    }

    setRanges = (e)=>{
        this.setState({[e.target.name+'Txt']: e.target.value});
    }

    checkValue = (e)=>{
        let state = {}
        if(/^(-?(\d*)(\.)?(\d+))+$/g.test(e.target.value)){
            state[e.target.name] = parseFloat(this.state[e.target.name+'Txt']);
            if(e.target.name==='from'){
                state['from'] = Math.min(state['from'],this.state.to-1);
                state['fromTxt'] = state['from'].toString();
            }else{
                state['to'] = Math.max(state['to'],this.state.from+1);
                state['toTxt'] = state['to'].toString();
            }
        }else
            state[e.target.name+'Txt'] = this.state[e.target.name].toString();
        
        this.setState(state);
    }

    render(){
        let chartBody = <Loader/>,chart = <Loader/>;
        if(this.state.load){
            if(!this.state.reload){
                let chartData = this.state.xy;
                chartData = chartData.map(value=> ({x: value.x.toFixed(2),y: value.y}));
                chart = (
                    <ResponsiveContainer width="100%" aspect={2}>
                        <LineChart className="centerFixed" data={chartData}>
                            <CartesianGrid stroke="#ccc" strokeDasharray="5 5"/>
                            <XAxis dataKey="x"/>
                            <YAxis />
                            <Tooltip />
                            <Line isAnimationActive={false} type="monotone" dataKey="y" stroke="#009FE3" dot={{ r: 2 }} activeDot={{ r: 4 }} />
                        </LineChart>
                    </ResponsiveContainer>
                );
            }
            chartBody = (
                <div className="s-row">
                    <h5>Wykres funkcji:</h5>
                    <div className="s-col s-small--12 s-large--2">
                        Wartość początkowa: <input onBlur={this.checkValue} onChange={this.setRanges} name="from" className="s-field" value={this.state.fromTxt} type="text" /><br/>
                        Wartość końcowa: <input onBlur={this.checkValue} onChange={this.setRanges} name="to" className="s-field" value={this.state.toTxt} type="text" /><br/>
                        Ilość kroków: {this.state.n}
                        <br/>
                        <InputRange maxValue={1000} minValue={10} onChange={value => this.setState({n: value})} value={this.state.n}/>
                        <br/>
                        <br/>
                        <button onClick={this.reload} className="s-button--secondary">Odśwież</button>
                    </div>
                    <div className="s-col s-small--12 s-medium--12 s-large--10">
                        {chart}
                    </div>
                </div>
			);
        }
        return(
            <Card type="success">
                {chartBody}
            </Card>
        );
    }

}

export default ChartView;
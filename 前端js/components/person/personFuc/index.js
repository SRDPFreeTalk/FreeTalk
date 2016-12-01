import React, { PropTypes, Component } from 'react'

export default class PersonMain extends Component{

	render(){
		const {features} = this.props;

		return(
			<div className="func">
	            <p>{features.name}</p>
	            <div className="right_func">
	                <img {{features.isNew}? ClassName="hide" : ClassName="show"} src="./img/new.png" alt="">
	                <p>{features.replyNum}</p>
	                <img className="jump_button" src="./img/jumpButton.gif" alt="">
	                <div className="clr"></div>
	            </div>
	        </div>
		)
	}
}
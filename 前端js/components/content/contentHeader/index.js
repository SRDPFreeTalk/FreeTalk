import React, { PropTypes, Component } from 'react'

export default class Content extends Component{
	render(){
		const {content} = this.props;
		return(
			<header className="content_head">
		        <a href="index.html">
		            <img src="./img/goBackButton.gif" alt="">
		        </a>
		        <h1>{content}</h1>
		        <div className="clr"></div>
		    </header>
		);
	}
}
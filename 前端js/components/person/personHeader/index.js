import React, { PropTypes, Component } from 'react'

export default class PersonHeader extends Component{
	render(){
		const { info } = this.props;
		return (
			<div className="person_head">
		        <a href="">编辑</a>
		        <div className="clr"></div>
		        <div className="information">
		            <img src={info.userAvatar} alt="">
		            <h3>{info.username}</h3>
		            <ul >
		                <li>关注</li>
		                <li>{info.concern}</li>
		                <div className="clr"></div>
		            </ul>
		            <ul>
		                <li>粉丝</li>
		                <li>{info.fans}</li>
		                <div className="clr"></div>
		            </ul>
		            <div className="clr"></div>
		            <p>{info.introduce}</p>
		        </div>
		    </div>
		)
	}
}
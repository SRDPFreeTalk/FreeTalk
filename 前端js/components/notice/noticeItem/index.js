import React, { PropTypes, Component } from 'react'

export default class NoticeItem extends Component{
	render(){
		const {content} = this.props;

		return (
			<div className="chat_box">
		        <input className="checkbox" type="checkbox">
		        <div className="move_box">
		            <img className="newMassage" {{content.isNew}? ClassName="hide" : ClassName="show"} src="./img/new.png" alt="">
		            <div className="title">
		                <img src={content.userAvatar} alt={content.username}>
		                <div className="subtitle">
		                    <h2>{content.username}</h2>
		                    <h5>{content.time}</h5>
		                    <p>{content.content}</p>
		                </div>
		                <div className="clr"></div>
		            </div>
		        </div>
		    </div>
		)
	}
}
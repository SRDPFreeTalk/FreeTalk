import React, { PropTypes, Component } from 'react'
import NoticeList from './noticeList'

export default class Notice extends Component{
	render(){
		return (
			<div className="notice">
				<div className="notice_head">
			        <a className="delete_box" >编辑</a>
			        <div className="clr"></div>
			    </div>
			    <NoticeList />
			    <div className="none">
			        <button className="back">取消</button>
			        <button className="del">删除</button>
			        <div className="clr"></div>
			    </div>
			</div>
		)
	}
}
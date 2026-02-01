#include <iostream>
using namespace std;
#include "Vector.h"
#include <bit>
Vector::Vector(int length)
{
	if (length <= 0) {
		length = 2; 
	}
	else if (!std::has_single_bit((unsigned int)length)) {
		length = std::bit_ceil((unsigned int)length);
	}

	data = new int[length];
	size = 0;
	capacity = length;

	for (int i = 0; i < length; i++) {
		data[i] = 0;
	}
}

Vector::Vector(const Vector& other)
{
	capacity = other.capacity;
	size = other.size;
	data = new int[capacity];
	for (int i = 0; i < size; i++)
		data[i] = other.data[i];

}
Vector::~Vector() {
	delete[] data;
}
Vector& Vector::operator=(const Vector& other) {
	if (this == &other) {
		return *this;
	}
	delete[] data;
	size = other.size;
	capacity = other.capacity;
	data = new int[capacity];
	for (int i = 0; i < size; i++) {
		data[i] = other.data[i];
	}
	return *this;
}
bool Vector::operator==(const Vector& other) const {
	if (this->size != other.size) {
		return false;
	}
	for (int i = 0; i < size; i++) {
		if (this->data[i] != other.data[i]) {
			return false; 
		}
	}
	return true; 
}
void Vector::print()
{
	cout << "\ncapacity: "<<capacity<<"\nsize: "<<size;
	for (int i = 0;i < size;i++)
		cout << data[i] << ",";

}

void Vector::insert(int num)
{
	if (size < capacity)
		data[size++] = num;
	else
	{
		capacity = capacity * 2;
		int *temp = new int[capacity];
		for (int i = 0;i < size;i++)
			temp[i] = data[i];
		delete data;
		data = temp;
		data[size++] = num;
	}
}

void Vector::assign(Vector& other)
{
	other = *this;
}

bool Vector::isEqual(Vector& other)
{
	return other == *this;
}

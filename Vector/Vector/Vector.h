#pragma once
class Vector
{
private:
	int* data;
	int capacity;
	int size;
public:
	Vector(int num = 2);
	~Vector() { delete[] data; }
	Vector(const Vector& other);
	Vector& operator=(const Vector& other);
	bool operator==(const Vector& other) const;
	void print();
	void insert(int num);
	int getCapacity() { return capacity; };
	int getSize() { return size; };
	void assign(Vector& other);
	bool isEqual(Vector& other);
};

